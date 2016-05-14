delimiter ;.
-- Table TBL_URLS
create table `TBL_URLS`
(
 `ID`             VARCHAR(32)    not null comment 'Unique identifier',
 `ORIGINALURL`    VARCHAR(2000)  not null comment 'Original url',
 `SHORTURL`       VARCHAR(100)   not null comment 'Short url',
 `CREATEDATE`     DATETIME       not null comment 'Url create date',
 `LASTACCESSDATE` DATETIME        comment 'Url last access date',
  constraint `PK_TBL_URLS_PRIMARY` primary key (`ID`)
)
    comment = 'Url recode table'
;.

delimiter ;.
  create  index `IDX_TBL_URLS_SHORTURL` on `TBL_URLS`(`SHORTURL`);.
