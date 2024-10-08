# Koshelekpy_Test
(Manual)
From the repository folder, 
run the command: docker-compose build, 
then run: docker-compose up. The container with PosgtreSQL and the application will be launched Asp.net.
To access the client sending the message, enter: http://localhost:5142/Client1, 
for a client receiving a real-time message: http://localhost:5142/Client2, 
for a client viewing messages for the last 10 minutes: http://localhost:5142/Client3

(Ddescription)
Сервис состоит из трех компонентов: 
● Web-сервер 
● SQL БД (желательно Postgresql) 
● 3 клиента (первый пишет сообщения, второй отображает их в реальном 
времени, третий позволяет просмотреть список сообщений за последнюю 
минуту). 
Все три клиентские части можно реализовать как отдельными web-приложениями, так 
и одним c разделение клиентов по url (как удобнее). 
Каждое сообщение состоит из текста до 128 символов, метки даты/времени 
(устанавливается на сервере) и порядкового номера (приходит от клиента). 
Схема работы системы следующая: 
● первый клиент пишет потоком произвольные (по контенту) сообщения в сервис 
(на одно сообщение один вызов к API) 
● сервис обрабатывает каждое сообщение, записывает его в базу и 
перенаправляет его второму клиенту по веб-сокету 
● второй клиент при считывает по веб-сокету поток сообщений от сервера и 
отображает их в порядке прихода с сервера (с отображением метки времени и 
порядкового номера) 
● через третий клиент пользователь может отобразить историю сообщений за 
последние 10 минут 
Серверная часть имеет REST или GraphQL (на выбор) API c 2 методами: 
● отправить одно сообщение 
● получить список сообщений за диапазон дат 
Желательно: сгенерить swagger-документацию (для REST-api). 
Перечень языков для разработки: C# или Go 
Архитектурные требования: 
* MVC или подобная 
* Слой DAL без использования ORM 
* Ведение логов, чтобы по ним можно было понять текущее состояние работы 
приложения (детальность логов на свое усмотрение, при этом качество логирования 
является одним из критериев оценки выполненного тестового задания)
