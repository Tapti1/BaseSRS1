SELECT distinct breed.title FROM breed inner join cat on
breed.breed_id=cat.breed_id inner join cat_owner on
cat_owner.cat_id=cat.cat_id inner join owner on
owner.owner_id=cat_owner.owner_id AND
owner.name='Карл Маркс'