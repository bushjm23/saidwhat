create schema saidwhat;
use saidwhat;

drop table if exists posts;
create table posts (
    idpost int not null primary key,
    created datetime    
) engine = innodb;