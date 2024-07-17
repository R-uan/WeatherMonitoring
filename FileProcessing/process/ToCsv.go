package process

import (
	"encoding/csv"
	"fileprocessing/models"
	"fmt"
	"log"
	"os"
	"path/filepath"
	"strconv"
	"time"
)

func ToCsv(data []models.WeatherReport) {
	dir := "../data/"
	fileName := GenerateFileName() 
	path := filepath.Join(dir, fileName+".csv")

	if _, err := os.Stat(dir); os.IsNotExist(err) {
		if err := os.Mkdir(dir, 0755); err != nil {
			panic(err)
		}
	}


	var addHeader bool = false;
	if _, err := os.Stat(path); os.IsNotExist(err) {
		log.Println("File not found. creating it.");
		file, err := os.Create(path);
		if(err != nil) { panic(err) }
		addHeader = true;
		file.Close();
	} 

	file, err := os.OpenFile(path, os.O_WRONLY|os.O_APPEND, 0644)
	if err != nil { panic(err) }
	AppendData(data, file, addHeader);
	defer file.Close();
}

func AppendData(data []models.WeatherReport, file *os.File, addHeader bool) {
	writer := csv.NewWriter(file);
	if(addHeader) {
		header := []string{"Location","Time","Temp","Humidity","Pressure","WindSpeed","Latitude","Longitude","WeatherId","Main","Description"};
		writer.Write(header);
	}
	defer writer.Flush();
	for _, report := range data {
		log.Printf("Appending data: %s", report.Location);
		row := []string{
			report.Location,
			strconv.Itoa(int(report.Time)),
			strconv.FormatFloat(float64(report.Temp), 'f', 2, 32),
			strconv.Itoa(int(report.Humidity)),
			strconv.Itoa(int(report.Pressure)),
			strconv.FormatFloat(float64(report.WindSpeed), 'f', 2, 32),
			strconv.FormatFloat(float64(report.Latitude), 'f', 5, 32),
			strconv.FormatFloat(float64(report.Longitude), 'f', 5, 32),
			strconv.Itoa(int(report.Weather.WeatherId)),
			report.Weather.Main,
			*report.Weather.Description,
		};
		writer.Write(row);
	}
}

func GenerateFileName() string {
	date := time.Now();
	ddMM := fmt.Sprintf("%d%d%d-report", date.Day(), date.Month(), date.Year());
	return ddMM;
}