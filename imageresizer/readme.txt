https://azure.microsoft.com/en-us/features/storage-explorer/

https://github.com/Azure/Azurite

docker pull arafato/azurite

docker run -d -t -p 10000:10000 -p 10001:10001 -p 10002:10002 -v ${pwd}:/opt/azurite/folder arafato/azurite

verificar se o container esta funcionando
docker ps
docker ps -a //mostra todos container
docker start fc "id as duas primeiras"


//o docker nao enxerga o ip 127.0.0.1 do emulador storage
docker inspect -f "{{ .NetworkSettings.IPAddress}}" 


--rodando o comando para gerar imagem // . (ponto é o diretorio onde estou)
--vai gerar a imagem contendo meu codigo fonte
docker build -t imageresizer .
docker images

--rodar o container para testar vai expor um endpoint para testar a aplicacao exemplo
--http://localhost:5005/api/images/resize/?name=imagem.big.jpg&format=png&width=500
docker run -d -p 5005:80 imageresizer

--saude e logs do container
docker logs 83 "inicial do nome do meu container"

--remover um container
docker rm -f 


-- para subir pode usar docker hub que é pago
-- mas vamos subir no azure container registries
--fazer o login no portal azure via powershell
docker login -u brasiliandevs -p HLGYlS72C1dB22uK/xWL4l4+3q0kaURg brasiliandevs.azurecr.io 

--fazer uma tag na imagem //endereco do meu servidor + o nome
docker tag imageresizer brasiliandevs.azurecr.io/imageresizer
docker push brasiliandevs.azurecr.io/imageresizer

--para fazer a imagem rodar ela precisa estar hospedada em algum lugar, ou seja criar um storage account
--ir em blob e criar o container "image" e "image-resized"

depois ir e criar o web app, publish linux e configure container e ir em azure container registry
-- apos criado o web app preciso apontar qual storage account minha imagem vai ficar
vai no web app / settings / configuration (preview) + new application settings 
StorageAccount__ConnectionString -> joga a connection string da storage account