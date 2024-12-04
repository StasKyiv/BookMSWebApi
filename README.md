1. If you want run project with docker compose - download:  "https://github.com/StasKyiv/BookMSDockerCompose.git", 
in folder with downloaded docker-compose file execute command "docker-compose up". All images will download from docker-hub.
#############
2. If You want run each project separatly -
   2.1 Download webapi: "https://github.com/StasKyiv/BookMSWebApi.git"
   for webapi you needn't any changes in settings for runing it locally.
   #######
   2.2 Download client: "https://github.com/StasKyiv/BookMSClient.git"
   for client app you need change the link to base url in ApiUrlAddress.cs to "http://localhost:5163".
   #######
   2.3 For database was used EntityFrameworkcore.InMemory, you needn't do any migrations.

   
   
