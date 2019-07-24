#---------------------------------
rm -fr bin/Release
dotnet publish -c Release

if [ -d "deploy" ]; then
  rm -fr deploy
fi


#---------------------------------
mkdir deploy
cp bin/Release/netcoreapp2.1/publish/* deploy/


#---------------------------------
cp -r wwwroot deploy/

#---------------------------------
ssh root@172.104.157.202 'systemctl stop kestrel-mysite5.service'

rsync \
  -r \
  -v \
  -c \
  -h --progress \
  --perms --chmod=a+rwx \
  deploy/ root@172.104.157.202:/var/www/mysite5 \
  --exclude=database \
  --exclude=keys \
  --delete

ssh root@172.104.157.202 'systemctl start kestrel-mysite5.service'
rm -rf deploy/*