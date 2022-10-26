create table if not exists dog (
    id varchar,
    avatar_url varchar,
    breed varchar,
    primary key (id)
);
create table if not exists "user" (
    id SERIAL PRIMARY KEY,
    dogs varchar[],
    first_name varchar,
    last_name varchar,
    avatar_url varchar,
    username varchar,
    "password" varchar,
    roles bigint[]
);
create table if not exists "role" (
    id bigint,
    name varchar,
    primary key (id)
);
insert into role (id, name)
select 100, 'admin' where not exists (select * from role where id=100);
insert into role (id, name)
select 200, 'user' where not exists(select * from role where id=200);
