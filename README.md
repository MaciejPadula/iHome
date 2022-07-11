# iHome
iHome is platform based on Azure SQL Database that maintains control over ESP8266 based intelligent home devices such as RGB light bulbs and remote accessed electric sockets.
# How it's working?
User creates account and rooms that can be shared with other users. To add device you have to press config button on it. Then you connect it to your local network. After that you will see your device in config window on site. After naming your device it will be available in one of your rooms: <br/>
![image](https://user-images.githubusercontent.com/50674232/178340555-8dc91f9a-f587-417f-958b-9467a3528389.png) <br/>
The device will begin exchanging data with Azure SQL Server.
# Sharing room
If you want to share a room with other user just click share button in top right corner of room card. Then provide friend's email address and you are done: <br/>
![image](https://user-images.githubusercontent.com/50674232/178340908-97843d82-60ef-4aba-bb9b-070d298af74b.png)
![image](https://user-images.githubusercontent.com/50674232/178341187-2bb65268-3d22-4c00-9287-fab7d88bd5c4.png)

# Hosting
This website will be hosted soon!

# Running
You are unable to run this page without Auth0 key and Azure SQL Database login informations
