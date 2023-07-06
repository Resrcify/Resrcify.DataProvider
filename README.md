# Titan.DataProvider
A rebuilt microservice version of Crinolo's stat calc which goal is to provide all the latest data needs.<br>
Now supports parsing of ***mods***, ***skills***, ***datacrons***, ***profile summary*** and more. DataProvider updates to the latest GameData file on a timely basis.<br>
DataProvider works directly with the [swgoh-comlink](https://github.com/swgoh-utils/swgoh-comlink), and accepts data in raw format.

For further help using this tool see below or you can access Swagger API documentation from a web browser at http://localhost:18000/swagger

## Environment Variables
|Environment Name|Description|Default Value|
|:-:|:-:|:-:|
|CLIENT_URL|The base URL of your Comlink instance|http://localhost|
|PORT|The port used by your Comlink instance|3200|
|ASPNETCORE_ENVIRONMENT|The environment you are currently running. Recommend running in Production when deployed and Development when testing (swagger will not work in production mode)|Development|


## How to run
### Example with docker-compose (recommended)
```yaml
version: "3.4"

services:
  dataprovider:
    image: ghcr.io/resrcify/titan.dataprovider:latest
    container_name: Titan.DataProvider
    environment:
      - CLIENT_URL=http://swgoh-comlink
      - PORT=3200
      - ASPNETCORE_ENVIRONMENT=Production
    restart: always
    depends_on:
      - swgoh-comlink
    ports:
      - 18000:18000

  swgoh-comlink:
    image: ghcr.io/swgoh-utils/swgoh-comlink:latest
    container_name: swgoh-comlink
    environment:
      - NODE_ENV=production
      - APP_NAME=titantestapp
    restart: always
    ports:
      - 3200:3200
```

### Example using docker comand line
```properties
docker pull ghcr.io/resrcify/titan.dataprovider:latest
docker stop dataprovider
docker rm dataprovider
docker run --name=dataprovider \
  -d \
  --restart always \
  --network swgoh-comlink \
  --env CLIENT_URL=http://localhost \
  --env PORT=3200 \
  --env ASPNETCORE_ENVIRONMENT=Production \
  -p 18000:18000 \
  ghcr.io/resrcify/titan.dataprovider:latest
```
### Example building and running with command line
- Download and install the required [runtime](https://dotnet.microsoft.com/en-us/download) (currently DataProvider is using .NET7)
- Pull the source code from git (use one of the below links depending on your preference)
  - git clone git@github.com:Resrcify/Titan.DataProvider.git
  - git clone https://github.com/Resrcify/Titan.DataProvider.git
- Navigate to src/API/Titan.DataProvider.API
- Edit the file appsettings.json to make it match your url (ClientUrl) and port (ClientPort) of Comlink.
- Run ``dotnet run``

## Endpoints

|Method|URL|Additional Information|Description|
|:-:|:-:|:-:|:-:|
|POST|/api/data/update|None|Performs a manual update of the Localization and GameData versions|
|GET|/api/data/base|Defaults to English|Retrieves the current cached GameData file provided in English|
|GET|/api/data/base/ENG_US|Language parameter in the route can be replace as per section "Language Routes"|Retrieves the current cached GameData file provided in the specified language|
|GET|/api/data/localization/|Defaults to English|Retrieves the current cached Localization file provided in English|
|GET|/api/data/localization/ENG_US|Language parameter in the route can be replace as per section "Language Routes"|Retrieves the current cached Localization file provided in the specified language|
|POST|/api/profile|Defaults to English. Has additional query parameters  as per section "Profile Query Params"|Expands the provided Player Profile as per query parameter settings. Defaults to English and performs all calculations if no query parameters are provided|
|POST|/api/profile/ENG_US|Language parameter in the route can be replace as per section "Language Routes". Furthermore, has additional query parameters as per section "Profile Query Params"|Expands the provided Player Profile as per query parameter and language settings. Performs all calculations if no query parameters are provided|

### Language Routes

|Route|
|:-:|
CHS_CN
CHT_CN
ENG_US
FRE_FR
GER_DE
IND_ID
ITA_IT
JPN_JP
KOR_KR
POR_BR
RUS_RU
SPA_XM
THA_TH
TUR_TR

### Profile Query Params

|Query Parameters|Default Value|Description|
|:-:|:-:|:-:|
|withoutGp|False| Perform GP calculations on each character / ship and perform Profile Summary of GP|
|withoutModStats|False|Include mod stats in stats calculation|
|withoutMods|False| Expand data on Mods for each character and perform a Profile Summary of Mods|
|withoutSkills|False|Expand data on Skills for each character / ship and perform a Profile Summary count of Omicron and Zeta |
|withoutDatacrons|False|Expand data on Datacrons and perform a Profile Datacron Summary|
|withStats|True|Perform unit stats calculation|
|definitionId|N/A|Specify a specific unit, using the comlinks provided definitionId, to perform calculations on (Example: MAGMATROOPER). Profile Summary will only be performed based on that unit|


## Postman Collection
[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/13493149-1cb56127-2649-4947-b080-d26e96025aef?action=collection%2Ffork&source=rip_markdown&collection-url=entityId%3D13493149-1cb56127-2649-4947-b080-d26e96025aef%26entityType%3Dcollection%26workspaceId%3D595e1fb2-07b7-4140-aa60-d7edb4fc027f)

## FAQ
- Q: Where is the GameData file saved?
- A: The GameData file is cached in memory.
- Q: Why cant I access the swagger documentation?
- A: There can be a number of reasons, but the most common one is probably that the wrong environment is used (Production). Swagger is disabled when using Production environment.
- Q: How often is the gamedata version updated / checked?
- A: Initially 15 seconds after startup and concurrently every 15 minutes.
