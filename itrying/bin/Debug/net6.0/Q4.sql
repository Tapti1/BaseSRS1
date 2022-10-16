SELECT AVG(cat.age) as num FROM cat inner join breed ON
cat.breed_id=breed.breed_id AND
breed.title='сфинкс'