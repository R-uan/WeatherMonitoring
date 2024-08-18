# Weather Monitoring - Documentation in progress

## Summary

The application utilizes of a task scheduler (Coravel) that requests the data about five different selected cities from the OpenWeatherAPI. Once retrieved, the data is first stored on a Redis database and sent through the message broker (RabbitMQ) to the Golang application that inserts the data on a file dedicated for each city. Later, the redis data is retrieved to create a daily report for each city average where it will be stored on a MongoDB database.
