SELECT title FROM family WHERE family_id NOT IN
(select o.family_id FROM owner_family AS o inner join cat_owner On
o.owner_id=cat_owner.owner_id)