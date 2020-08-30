# Installing CommunityBot

## Contacts

* Terry Ott (Terry.Ott@gmail.com)
* JustinS83 (JustinS83.waze@gmail.com)

## AWS

### Alpine

#### AMI

`alpine-ami-edge-x86_64-20200425232123`

AMI ID: `ami-097be5ea1a5c7b6ce`

#### Instance type

Starting with `t3.nano`.  Seeing if we can do a build and working up.

#### Instance details

Put in us-east-1d, same region as RDS.


#### Storage

5GB of magnetic.


#### Logging in

`ssh -i [communitybot account pem] alpine@[instance ip address]`

#### Docker

Let's see what version of Docker that [Alpine 3.12 comes with](https://wiki.alpinelinux.org/wiki/Docker).

```
$ sudo apk add docker
$ docker version
Client:
 Version:           19.03.12
 API version:       1.40
 Go version:        go1.14.4
 Git commit:        48a66213fe1747e8873f849862ff3fb981899fc6
 Built:             Wed Jul  1 17:10:48 2020
 OS/Arch:           linux/amd64
 Experimental:      false
$ sudo rc-update add docker boot
$ sudo service docker start
$ sudo addgroup ${USER} docker
```

Make sure to fully log out of the host and back in to refresh group
membership lists.

#### Docker Compose

```
$ sudo apk add docker-compose
$ docker-compose version
docker-compose version 1.26.2, build unknown
docker-py version: 4.3.1
CPython version: 3.8.5
OpenSSL version: OpenSSL 1.1.1g  21 Apr 2020
```

#### Check out bot code

```
$ sudo apk add git
$ mkdir git
$ cd git
$ git clone https://gitlab.com/SixbucksSolutions/CommunityBot.git
```

#### Building with just 3.1 to confirm it works

```
$ docker-compose build
```

### Success

Able to build self-contained in 512MB RAM without bot running

### Next optimization: Globalization invariant mode

More info [here](https://github.com/dotnet/corefx/blob/master/Documentation/architecture/globalization-invariant-mode.md)

Was already in its own ItemGroup.  Cool.

### Next optimization: trim

More info [here](https://docs.microsoft.com/en-us/dotnet/core/deploying/trim-self-contained).

Bombed out, probably due to not enough memory.

### Increase instance size

Shut down, increase instance size to t3.micro.

### Retry trim

Built in 65 seconds, nice!

### Next optimization: aggressive trim

More info [here](https://docs.microsoft.com/en-us/dotnet/core/deploying/trim-self-contained)

Built in 85 seconds.

### Wipe images, time FULL build

1 minute 50 seconds.  Not bad.


### Next optimization: Ready to run

More info [here](https://docs.devexpress.com/WPF/401276/dotnet-core-support/deploy-netcore-application).

Wiped all processes/images.

Build time: 2 minutes 30 seconds -- that is expensive, but a one time cost


### Next optimization: runtime deps image

Now that we're self contained, we don't need full runtime image.

Size with runtime: 180MB

Size with runtime-deps: 103MB

Build time: 7 minutes, 5 seconds. Ouch

### Final optimization: single file

More info [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish).

Image size before option: 103MB

Shell in: `docker run -it --entrypoint /bin/sh communitybot:alpinetest-dotnet-3.1-alpine`

Files before option: `find /app -type f | wc -l` gave us 135 files

Size after option: 103MB.  Didn't save any space, just easier on human eyes.

Files after option: wow.  Nice.

```
$ docker run -it --rm --entrypoint /bin/sh communitybot:alpinetest-dotnet-3.1-alpine
1-alpine
/bot # ls -alF
total 91060
drwxr-xr-x    1 root     root          4096 Aug 30 20:45 ./
drwxr-xr-x    1 root     root          4096 Aug 30 20:45 ../
-rwxr-xr-x    1 root     root      93146768 Aug 30 20:44 WazeBotDiscord*
-rw-r--r--    1 root     root         85700 Aug 30 20:43 WazeBotDiscord.pdb
```

Frigging two files.

The runtime-deps image is 9.86MB.  Like less than 10MB.

We're adding our 93MB of self-contained, maximum pre-compiled executable to an impressively bare-bones image.

Build time (no clean): 2 minutes, 8 seconds

### Wipe all processes, images, do full clean build before we switch to t3a.micro

Total time: 2 minutes, 24 seconds.

I can handle that.

### t3a.micro full build

Build time after full clean: 2 minutes, 43 seconds.

As expected, still 103MB.  That should be byte-identical to t3.micro build.
