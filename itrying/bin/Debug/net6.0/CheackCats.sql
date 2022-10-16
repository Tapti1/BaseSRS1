with t as(SELECT cat_id,count(owner.name),family.title from cat_owner inner JOIN owner 
on owner.owner_id = cat_owner.owner_id inner join owner_family 
on cat_owner.owner_id = owner_family.owner_id inner join family 
on family.family_id = owner_family.family_id
group by cat_id,family.title
having count(owner.name)>1)
Select * from t
group by cat_id