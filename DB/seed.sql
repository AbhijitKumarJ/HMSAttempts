INSERT INTO public.roles(name) VALUES ('Admin');
INSERT INTO public.roles(name) VALUES ('Receptionist');
INSERT INTO public.roles(name) VALUES ('Nurse');
INSERT INTO public.roles(name) VALUES ('Doctor');
INSERT INTO public.roles(name) VALUES ('BillingClerk');


CREATE EXTENSION IF NOT EXISTS pgcrypto;

INSERT INTO public.users(username, password_hash, is_active) 
VALUES ('admin1', crypt('password', gen_salt('bf')), true);

INSERT INTO public.users(username, password_hash, is_active) 
VALUES ('receptionist1', crypt('password', gen_salt('bf')), true);

INSERT INTO public.users(username, password_hash, is_active) 
VALUES ('nurse1', crypt('password', gen_salt('bf')), true);

INSERT INTO public.users(username, password_hash, is_active) 
VALUES ('doctor1', crypt('password', gen_salt('bf')), true);

INSERT INTO public.users(username, password_hash, is_active) 
VALUES ('billingclerk1', crypt('password', gen_salt('bf')), true);



INSERT INTO public.user_roles(user_id, role_id)	VALUES (1, 1);
INSERT INTO public.user_roles(user_id, role_id)	VALUES (2, 2);
INSERT INTO public.user_roles(user_id, role_id)	VALUES (3, 3);
INSERT INTO public.user_roles(user_id, role_id)	VALUES (4, 4);
INSERT INTO public.user_roles(user_id, role_id)	VALUES (5, 5);


SELECT id, name FROM public.roles;

select * from public.users;

select * from public.user_roles;

SELECT (password_hash = crypt('password', password_hash)) AS password_matches
FROM public.users
WHERE username = 'admin1';
	

