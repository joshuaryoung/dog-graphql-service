create table if not exists dog (
    id varchar,
    avatar_url varchar,
    breed varchar,
    primary key (id)
);
create table if not exists "user" (
    id bigint,
    dogs varchar[],
    first_name varchar,
    last_name varchar,
    avatar_url varchar,
    primary key (id)
);