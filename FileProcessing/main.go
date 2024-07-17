package main

import (
	"encoding/json"
	"fileprocessing/models"
	"fileprocessing/process"
	"log"

	amqp "github.com/rabbitmq/amqp091-go"
)

func main() {
	conn, err := amqp.Dial("amqp://user:password@localhost:5672/");
	failOnError(err, "Failed to connect to RabbitMQ")
	defer conn.Close();

	ch, err := conn.Channel();
	failOnError(err, "Failed to create channel.")
	defer ch.Close();

	q, err := ch.QueueDeclare("hourly-report", false, false, false, false, nil);
	failOnError(err, "Failed to declare a queue.")

	msgs, err := ch.Consume(q.Name, "", true, false, false, false, nil);
	failOnError(err, "failed to register a consumer.")

	var forever chan struct{}

	go func() {
		for d := range msgs {
			var report []models.WeatherReport;
			err := json.Unmarshal(d.Body, &report);
			if(err != nil) { panic(err) }
			log.Println("Received hourly report.")
			process.ToCsv(report);
		}
	}()

	log.Printf(" [*] Waiting for messages. To exit press CTRL+C")
	<-forever
}

func failOnError(err error, msg string) {
	if err != nil {
		log.Panicf("%s: %s", msg, err)
	}
}