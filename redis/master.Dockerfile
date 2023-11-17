FROM redis
COPY redis/master_conf.conf /usr/local/etc/redis/redis.conf
COPY redis/sentinel.conf /usr/local/etc/redis/sentinel.conf
CMD [ "redis-server", "/usr/local/etc/redis/redis.conf"]