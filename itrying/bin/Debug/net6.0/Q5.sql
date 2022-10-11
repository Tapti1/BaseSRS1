WITH t AS(
SELECT family.title,COUNT(cat.cat_id) AS num FROM family inner JOIN owner_family ON
family.family_id=owner_family.family_id inner join cat_owner ON
cat_owner.owner_id=owner_family.owner_id inner join cat On
cat.cat_id=cat_owner.cat_id inner join pride on 
pride.pride_id=cat.pride_id AND pride.title='Бесславные ублюдки'
group by family.title
)
SELECT title FROM t WHERE num=(SELECT MAX(num) FROM t)