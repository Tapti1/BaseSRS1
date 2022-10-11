SELECT COUNT(distinct cat_owner.cat_id) FROM cat_owner inner join owner_family ON 
owner_family.owner_id=cat_owner.owner_id inner join family On
family.family_id=owner_family.family_id AND family.title='Энгельс'
