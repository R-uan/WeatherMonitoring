package models

type WeatherReport struct {
	Location  string  `json:"location"`
	Time      int64   `json:"time"`
	Temp      float32 `json:"temp"`
	Humidity  int     `json:"humidity"`
	Pressure  int     `json:"pressure"`
	WindSpeed float32 `json:"windSpeed"`
	Latitude  float32 `json:"latitude"`
	Longitude float32 `json:"longitude"`
	Weather   Weather `json:"weather"`
}

type Weather struct {
	WeatherId   int     `json:"weatherId"`
	Main        string  `json:"main"`
	Description *string `json:"description"`
}
