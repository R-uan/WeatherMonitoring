# Weather Monitoring - Documentation in progress
## Basic Summary
### C# (ASP.NET Core)
Weather application that periodically requests data from an third party api and makes a daily summary based on the hourly requests made thought the day and stores it on a Mongo database. 
### Golang
There's some Golang here because I wanted to try RabbitMQ to do real time communication between applications and get familiarized with the language. The program receives 
the data trough the rabbit channel and appends it on the specific CSV file from that day.
### Rust
Maybe I'll find a way to use rust in this, maybe not. Idk ¯\\_(ツ)_/¯
