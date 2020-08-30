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

