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


### To do

- Mostrar persistencia com o Redis em outro endpoint   
- Verificar se é viável mostra outros usos além do cache de um objeto
