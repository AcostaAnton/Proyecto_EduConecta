#!/bin/sh
set -e

mkdir -p /data/db
mongod --dbpath /data/db --bind_ip 127.0.0.1 --port 27017 --fork --logpath /var/log/mongod.log

exec dotnet EduConecta.Api.dll