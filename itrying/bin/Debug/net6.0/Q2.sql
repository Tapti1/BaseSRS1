SELECT title FROM pride WHERE pride_id NOT IN 
(select pride_id from cat)