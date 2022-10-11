SELECT COUNT(owner_family.owner_id) FROM owner_family inner join family ON
family.family_id=owner_family.family_id AND family.title='Рабинович'