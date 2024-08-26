FROM postgres:latest

ENV POSTGRES_DB=KoshelekPy_Test
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=super

EXPOSE 5432

# Если у вас есть SQL-скрипты для инициализации базы данных, их можно скопировать в контейнер и выполнить
COPY init.sql /docker-entrypoint-initdb.d/
