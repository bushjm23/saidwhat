create schema saidwhat;
use saidwhat;

create user 'saidwhatuser'@'%' identified by 'c4ntb3li3v1e1s4idthat';
grant select, insert, update, delete, execute on saidwhat.* to 'saidwhatuser'@'%';
flush privileges;

drop table if exists posts;
create table posts (
    idpost int not null auto_increment primary key,
    created datetime not null,
    message varchar(1000) not null default '',
    index idx_posts_created (created desc)
) engine = innodb;