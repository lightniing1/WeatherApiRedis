# WeatherApi

## Docker
Para fazer a build no docker:
`docker-compose -f docker-compose.yml up`

Para interagir com a API no navegador:
`https://localhost:5001/swagger/index.html`


## Redis
Para interagir com o redis dentro do docker: 
`docker exec -it *Container ID* redis-cli`

Saber quais chaves foram criadas pela redis-cli:  
`KEYS *`

Tipo de conteudo dentro da chave  
`TYPE "nomedakey"`

Saber o valor da chave

    for "string": get <key>
    for "hash": hgetall <key>
    for "list": lrange <key> 0 -1
    for "set": smembers <key>
    for "zset": zrange <key> 0 -1 withscores


## Replicação

Primeiro é necessário executar os seguintes comandos nas replicas

Replica 1 (redis2)
	`docker exec -it weatherapi-redis2-1 redis-cli replicaof 172.16.238.2 6379`

Replica 2 (redis3)
	`docker exec -it weatherapi-redis3-1 redis-cli replicaof 172.16.238.2 6379`

Esse comando executa o `redis-cli` dentro do docker com o argumento `replicaof`. O argumento indica a replica qual é a instância principal que deve ser replicada


As instancia configuram-se automaticamente e nenhum comando a mais é necessário.

Para verificar que a instancia principal tem replicas configuradas, execute o seguinte comando no container redis principal
`docker exec -it weatherapi-redis-1 redis-cli info replication`


Quando o cache de algum item for feito na instancia principal, é enviado o comando de sincronização, podendo ser visto nos logs


## Configurando o sentinel

É essencial que, no mínimo, exista 3 instancias do sentinel rodando para as decisões serem tomadas. O sentinel é programado para que uma MAIORIA tome a decisão de promoção de uma instancia. Sem um consenso (quorum = 1), as decisões não são tomadas


A porta 26379 (ou a que foi configurada manualmente para executar o sentinel) DEVE ESTAR aberta
É através dessa porta que as instancias são descobertas e configuradas automaticamente


Em cada uma das instancias sentinel, execute o seguinte comando para executar o sentinel:

`redis-sentinel /usr/local/etc/redis/sentinel.conf`


É necessária também uma alteração no código para que ele conecte-se as instancias sentinel em vez da redis


Se uma instancia cair, o quorum vai auto promover uma instancia replica para main. Se a main voltar, ela vai voltar como uma replica e vai 
sincronizar os novos dados com as outras instancia automaticamente. Entretanto, os dados antigos que estavam na main que não foram replicados para as replicas durante a queda (a replicação é assincrona! não é imediata!


O arquivo de configuração do sentinel contido nas instancias também é alterado automáticamente

