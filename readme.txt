REST API. Description

The service provides basic functions for working with the online store.

There are four method: GET, POST, PUT, DELETE. The last two require taking Id.

There is a GET-method for filtering goods which can take JSON-string in "api/products/".
Structure of JSON-string:
{
	"name" : nameOfProduct,
	"producer" : [arrayProducers]
	"categories" : [arrayCategories],
	"price" : [minPrice,maxPrice],
}