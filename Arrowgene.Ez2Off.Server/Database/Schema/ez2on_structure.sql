CREATE TABLE IF NOT EXISTS `setting` (
  `key`   VARCHAR(255) NOT NULL,
  `value` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`key`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `account` (
  `id`               INT(11)      NOT NULL AUTO_INCREMENT,
  `name`             VARCHAR(17)  NOT NULL,
  `normal_name`      VARCHAR(17)  NOT NULL,
  `hash`             VARCHAR(255) NOT NULL,
  `mail`             VARCHAR(255) NOT NULL,
  `mail_verified`    TINYINT(1)   NOT NULL,
  `mail_verified_at` DATETIME               DEFAULT NULL,
  `mail_token`       VARCHAR(255)           DEFAULT NULL,
  `password_token`   VARCHAR(255)           DEFAULT NULL,
  `state`            INT(11)      NOT NULL,
  `last_login`       DATETIME               DEFAULT NULL,
  `created`          DATETIME     NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_account_name` (`name`),
  UNIQUE KEY `uq_account_normal_name` (`normal_name`),
  UNIQUE KEY `uq_account_mail` (`mail`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `incident` (
  `id`          INT(11)       NOT NULL AUTO_INCREMENT,
  `account_id`  INT(11)       NOT NULL,
  `type`        INT(11)       NOT NULL,
  `description` VARCHAR(1024) NOT NULL,
  `created`     DATETIME      NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_incident_account_id` (`account_id`),
  CONSTRAINT `fk_incident_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;
  
CREATE TABLE IF NOT EXISTS `identification` (
  `id`          INT(11)          NOT NULL AUTO_INCREMENT,
  `account_id`  INT(11)          NOT NULL,
  `ip`          VARCHAR(45)      NOT NULL,
  `hardware_id` VARCHAR(255)               DEFAULT NULL,  
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_ip_account_id_ip_hardware_id` (`account_id`, `ip`, `hardware_id`),  
  KEY `fk_identification_account_id` (`account_id`),
  CONSTRAINT `fk_identification_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_character` (
  `id`             INT(11)             NOT NULL,
  `name`           VARCHAR(45)         NOT NULL,
  `sex`            INT(11)             NOT NULL,
  `level`          TINYINT(1) UNSIGNED NOT NULL,
  `ruby_exr`       INT(11)             NOT NULL,
  `street_exr`     INT(11)             NOT NULL,
  `club_exr`       INT(11)             NOT NULL,
  `exp`            INT(11)             NOT NULL,
  `coin`           INT(11)             NOT NULL,
  `cash`           INT(11)             NOT NULL,
  `max_combo`      INT(11)             NOT NULL,
  `ruby_wins`      INT(11)             NOT NULL,
  `street_wins`    INT(11)             NOT NULL,
  `club_wins`      INT(11)             NOT NULL,
  `ruby_loses`     INT(11)             NOT NULL,
  `street_loses`   INT(11)             NOT NULL,
  `club_loses`     INT(11)             NOT NULL,
  `premium`        INT(11)             NOT NULL,
  `dj_points`      INT(11)             NOT NULL,
  `dj_points_plus` INT(11)             NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_ez2on_character_name` (`name`),
  CONSTRAINT `fk_ez2on_character_id` FOREIGN KEY (`id`) REFERENCES `account` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_song` (
  `id`               INT(11)               NOT NULL,
  `name`             VARCHAR(255)          NOT NULL,
  `category`         INT(11)               NOT NULL,
  `duration`         VARCHAR(255)          NOT NULL,
  `bpm`              INT(11)               NOT NULL,
  `file_name`        VARCHAR(255)          NOT NULL,
  `ruby_ez_exr`      INT(11)               NOT NULL,
  `ruby_ez_notes`    INT(11)               NOT NULL,
  `ruby_nm_exr`      INT(11)               NOT NULL,
  `ruby_nm_notes`    INT(11)               NOT NULL,
  `ruby_hd_exr`      INT(11)               NOT NULL,
  `ruby_hd_notes`    INT(11)               NOT NULL,
  `ruby_shd_exr`     INT(11)               NOT NULL,
  `ruby_shd_notes`   INT(11)               NOT NULL,
  `street_ez_exr`    INT(11)               NOT NULL,
  `street_ez_notes`  INT(11)               NOT NULL,
  `street_nm_exr`    INT(11)               NOT NULL,
  `street_nm_notes`  INT(11)               NOT NULL,
  `street_hd_exr`    INT(11)               NOT NULL,
  `street_hd_notes`  INT(11)               NOT NULL,
  `street_shd_exr`   INT(11)               NOT NULL,
  `street_shd_notes` INT(11)               NOT NULL,
  `club_ez_exr`      INT(11)               NOT NULL,
  `club_ez_notes`    INT(11)               NOT NULL,
  `club_nm_exr`      INT(11)               NOT NULL,
  `club_nm_notes`    INT(11)               NOT NULL,
  `club_hd_exr`      INT(11)               NOT NULL,
  `club_hd_notes`    INT(11)               NOT NULL,
  `club_shd_exr`     INT(11)               NOT NULL,
  `club_shd_notes`   INT(11)               NOT NULL,
  `measure_scale`    FLOAT                 NOT NULL,
  `judgment_kool`    TINYINT(1)   UNSIGNED NOT NULL,
  `judgment_cool`    TINYINT(1)   UNSIGNED NOT NULL,
  `judgment_good`    TINYINT(1)   UNSIGNED NOT NULL,
  `judgment_miss`    TINYINT(1)   UNSIGNED NOT NULL,
  `gauge_cool`       FLOAT                 NOT NULL,
  `gauge_good`       FLOAT                 NOT NULL,
  `gauge_miss`       FLOAT                 NOT NULL,
  `gauge_fail`       FLOAT                 NOT NULL,
  PRIMARY KEY (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;
  
CREATE TABLE IF NOT EXISTS `ez2on_radiomix` (
  `id`                  INT(11)      NOT NULL,
  `b`                   INT(11)      NOT NULL,
  `c`                   INT(11)      NOT NULL,
  `d`                   INT(11)      NOT NULL,
  `e`                   INT(11)      NOT NULL,
  `song_1_id`           INT(11)      NOT NULL,
  `song_1_ruby_notes`   INT(11)      NOT NULL,
  `song_1_street_notes` INT(11)      NOT NULL,
  `song_1_club_notes`   INT(11)      NOT NULL,
  `song_1_club8k_notes` INT(11)      NOT NULL,
  `song_2_id`           INT(11)      NOT NULL,
  `song_2_ruby_notes`   INT(11)      NOT NULL,
  `song_2_street_notes` INT(11)      NOT NULL,
  `song_2_club_notes`   INT(11)      NOT NULL,
  `song_2_club8k_notes` INT(11)      NOT NULL,
  `song_3_id`           INT(11)      NOT NULL,
  `song_3_ruby_notes`   INT(11)      NOT NULL,
  `song_3_street_notes` INT(11)      NOT NULL,
  `song_3_club_notes`   INT(11)      NOT NULL,
  `song_3_club8k_notes` INT(11)      NOT NULL,
  `song_4_id`           INT(11)      NOT NULL,
  `song_4_ruby_notes`   INT(11)      NOT NULL,
  `song_4_street_notes` INT(11)      NOT NULL,
  `song_4_club_notes`   INT(11)      NOT NULL,
  `song_4_club8k_notes` INT(11)      NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_ez2on_radiomix_song_1_id` (`song_1_id`),
  KEY `fk_ez2on_radiomix_song_2_id` (`song_2_id`),
  KEY `fk_ez2on_radiomix_song_3_id` (`song_3_id`),
  KEY `fk_ez2on_radiomix_song_4_id` (`song_4_id`),
  CONSTRAINT `fk_ez2on_radiomix_id` FOREIGN KEY (`id`) REFERENCES `ez2on_song` (`id`),
  CONSTRAINT `fk_ez2on_radiomix_song_1_id` FOREIGN KEY (`song_1_id`) REFERENCES `ez2on_song` (`id`),
  CONSTRAINT `fk_ez2on_radiomix_song_2_id` FOREIGN KEY (`song_2_id`) REFERENCES `ez2on_song` (`id`),
  CONSTRAINT `fk_ez2on_radiomix_song_3_id` FOREIGN KEY (`song_3_id`) REFERENCES `ez2on_song` (`id`),
  CONSTRAINT `fk_ez2on_radiomix_song_4_id` FOREIGN KEY (`song_4_id`) REFERENCES `ez2on_song` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;
  
CREATE TABLE IF NOT EXISTS `ez2on_game` (
  `id`         INT(11)      NOT NULL AUTO_INCREMENT,
  `song_id`    INT(11)      NOT NULL,
  `group_type` INT(11)      NOT NULL,
  `type`       INT(11)      NOT NULL,
  `name`       VARCHAR(255) NOT NULL,
  `created`    DATETIME     NOT NULL,
  `mode`       INT(11)      NOT NULL,
  `difficulty` INT(11)      NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_ez2on_game_song_id` (`song_id`),
  CONSTRAINT `fk_ez2on_game_song_id` FOREIGN KEY (`song_id`) REFERENCES `ez2on_song` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_item` (
  `id`              INT(11)               NOT NULL,
  `name`            VARCHAR(255)          NOT NULL,
  `effect`          VARCHAR(255)          NOT NULL,
  `image`           VARCHAR(255)          NOT NULL,
  `duration`        INT(11)               NOT NULL,
  `price`           INT(11)               NOT NULL,
  `level`           INT(11)               NOT NULL,
  `exp_plus`        INT(11)               NOT NULL,
  `coin_plus`       INT(11)               NOT NULL,
  `hp_plus`         INT(11)               NOT NULL,
  `resilience_plus` INT(11)               NOT NULL,
  `defense_plus`    INT(11)               NOT NULL,
  `type`            INT(11)               NOT NULL,
  `enabled`         TINYINT(1)   UNSIGNED NOT NULL,
  `currency`        INT(11)               NOT NULL,
  PRIMARY KEY (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_inventory` (
  `id`            INT(11)  NOT NULL AUTO_INCREMENT,
  `character_id`  INT(11)  NOT NULL,
  `item_id`       INT(11)  NOT NULL,
  `purchase_date` DATETIME NOT NULL,
  `slot`          INT(11)  NOT NULL,
  `equipped`      INT(11)  NOT NULL,
  `equip_date`    DATETIME           DEFAULT NULL,
  `expire_date`   DATETIME           DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_ez2on_inventory_expire_date` (`expire_date`),
  KEY `fk_ez2on_inventory_character_id` (`character_id`),
  KEY `fk_ez2on_inventory_item_id` (`item_id`),
  CONSTRAINT `fk_ez2on_inventory_character_id` FOREIGN KEY (`character_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_inventory_item_id` FOREIGN KEY (`item_id`) REFERENCES `ez2on_item` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_quest` (
  `id`      INT(11)      NOT NULL,
  `title`   VARCHAR(255) NOT NULL,
  `mission` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_score` (
  `id`           INT(11)             NOT NULL AUTO_INCREMENT,
  `game_id`      INT(11)             NOT NULL,
  `character_id` INT(11)             NOT NULL,
  `song_id`      INT(11)             NOT NULL,
  `difficulty`   INT(11)             NOT NULL,
  `stage_clear`  TINYINT(1) UNSIGNED NOT NULL,
  `max_combo`    INT(11)             NOT NULL,
  `kool`         INT(11)             NOT NULL,
  `cool`         INT(11)             NOT NULL,
  `good`         INT(11)             NOT NULL,
  `miss`         INT(11)             NOT NULL,
  `fail`         INT(11)             NOT NULL,
  `raw_score`    INT(11)             NOT NULL,
  `rank`         INT(11)             NOT NULL,
  `total_notes`  INT(11)             NOT NULL,
  `combo_type`   INT(11)             NOT NULL,
  `total_score`  INT(11)             NOT NULL,
  `note_effect`  INT(11)             NOT NULL,
  `fade_effect`  INT(11)             NOT NULL,
  `created`      DATETIME            NOT NULL,
  `mode`         INT(11)             NOT NULL,
  `incident`     TINYINT(1) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_ez2on_score_game_id` (`game_id`),
  KEY `fk_ez2on_score_character_id` (`character_id`),
  KEY `fk_ez2on_score_song_id` (`song_id`),
  CONSTRAINT `fk_ez2on_score_game_id` FOREIGN KEY (`game_id`) REFERENCES `ez2on_game` (`id`),
  CONSTRAINT `fk_ez2on_score_character_id` FOREIGN KEY (`character_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_score_song_id` FOREIGN KEY (`song_id`) REFERENCES `ez2on_song` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_score_incident` (
  `score_id`    INT(11) NOT NULL,
  `incident_id` INT(11) NOT NULL,  
  UNIQUE KEY `uq_ez2on_score_incident_score_id_incident_id` (`score_id`, `incident_id`),  
  KEY `fk_ez2on_score_incident_score_id` (`score_id`),
  KEY `fk_ez2on_score_incident_incident_id` (`incident_id`),
  CONSTRAINT `fk_ez2on_score_incident_score_id` FOREIGN KEY (`score_id`) REFERENCES `ez2on_score` (`id`),
  CONSTRAINT `fk_ez2on_score_incident_incident_id` FOREIGN KEY (`incident_id`) REFERENCES `incident` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_setting` (
  `character_id`      INT(11)             NOT NULL,
  `ruby_key_on_1`     TINYINT(1) UNSIGNED NOT NULL,
  `ruby_key_on_2`     TINYINT(1) UNSIGNED NOT NULL,
  `ruby_key_on_3`     TINYINT(1) UNSIGNED NOT NULL,
  `ruby_key_on_4`     TINYINT(1) UNSIGNED NOT NULL,
  `ruby_key_ac_1`     TINYINT(1) UNSIGNED NOT NULL,
  `ruby_key_ac_2`     TINYINT(1) UNSIGNED NOT NULL,
  `ruby_key_ac_3`     TINYINT(1) UNSIGNED NOT NULL,
  `ruby_key_ac_4`     TINYINT(1) UNSIGNED NOT NULL,
  `street_key_on_1`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_on_2`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_on_3`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_on_4`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_on_5`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_ac_1`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_ac_2`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_ac_3`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_ac_4`   TINYINT(1) UNSIGNED NOT NULL,
  `street_key_ac_5`   TINYINT(1) UNSIGNED NOT NULL,
  `club_key_on_1`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_on_2`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_on_3`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_on_4`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_on_5`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_on_6`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_ac_1`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_ac_2`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_ac_3`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_ac_4`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_ac_5`     TINYINT(1) UNSIGNED NOT NULL,
  `club_key_ac_6`     TINYINT(1) UNSIGNED NOT NULL,
  `volume_menu_music` TINYINT(1) UNSIGNED NOT NULL,
  `volume_menu_sfx`   TINYINT(1) UNSIGNED NOT NULL,
  `volume_game_music` TINYINT(1) UNSIGNED NOT NULL,
  `volume_game_sfx`   TINYINT(1) UNSIGNED NOT NULL,
  `bga_settings`      TINYINT(1) UNSIGNED NOT NULL,
  `skin_position`     TINYINT(1) UNSIGNED NOT NULL,
  `skin_type`         TINYINT(1) UNSIGNED NOT NULL,
  PRIMARY KEY (`character_id`),
  KEY `fk_ez2on_setting_character_id` (`character_id`),
  CONSTRAINT `fk_ez2on_setting_character_id` FOREIGN KEY (`character_id`) REFERENCES `ez2on_character` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_message` (
  `id`          INT(11)             NOT NULL AUTO_INCREMENT,
  `sender_id`   INT(11)             NOT NULL,
  `receiver_id` INT(11)             NOT NULL,
  `content`     VARCHAR(255)        NOT NULL,
  `send_at`     DATETIME            NOT NULL,
  `read`        TINYINT(1) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_ez2on_message_sender_id` (`sender_id`),
  KEY `fk_ez2on_message_receiver_id` (`receiver_id`),
  CONSTRAINT `fk_ez2on_message_sender_id` FOREIGN KEY (`sender_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_message_receiver_id` FOREIGN KEY (`receiver_id`) REFERENCES `ez2on_character` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_friend` (
  `id`                  INT(11) NOT NULL AUTO_INCREMENT,
  `character_id`        INT(11) NOT NULL,
  `friend_character_id` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_ez2on_friend_character_id_friend_character_id` (`character_id`, `friend_character_id`),
  KEY `fk_ez2on_friend_character_id` (`character_id`),
  KEY `fk_ez2on_friend_friend_character_id` (`friend_character_id`),
  CONSTRAINT `fk_ez2on_friend_character_id` FOREIGN KEY (`character_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_friend_friend_character_id` FOREIGN KEY (`friend_character_id`) REFERENCES `ez2on_character` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `ez2on_rank` (
  `id`       INT(11)             NOT NULL AUTO_INCREMENT,
  `game_id`  INT(11)             NOT NULL,
  `score_id` INT(11)             NOT NULL,
  `ranking`  TINYINT(1) UNSIGNED NOT NULL,
  `team`     INT(11)             NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_ez2on_rank_game_id` (`game_id`),
  KEY `fk_ez2on_rank_score_id` (`score_id`),
  CONSTRAINT `fk_ez2on_rank_game_id` FOREIGN KEY (`game_id`) REFERENCES `ez2on_game` (`id`),
  CONSTRAINT `fk_ez2on_rank_score_id` FOREIGN KEY (`score_id`) REFERENCES `ez2on_score` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;
  
CREATE TABLE IF NOT EXISTS `ez2on_gift` (
  `id`          INT(11)             NOT NULL AUTO_INCREMENT,
  `item_id`     INT(11)             NOT NULL,
  `sender_id`   INT(11)             NOT NULL,
  `receiver_id` INT(11)             NOT NULL,
  `send_at`     DATETIME            NOT NULL,
  `read`        TINYINT(1) UNSIGNED NOT NULL,
  `expire_date` DATETIME                      DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_ez2on_gift_expire_date` (`expire_date`),
  KEY `fk_ez2on_gift_item_id` (`item_id`),
  KEY `fk_ez2on_gift_sender_id` (`sender_id`),
  KEY `fk_ez2on_gift_receiver_id` (`receiver_id`),
  CONSTRAINT `fk_ez2on_gift_item_id` FOREIGN KEY (`item_id`) REFERENCES `ez2on_item` (`id`),
  CONSTRAINT `fk_ez2on_gift_sender_id` FOREIGN KEY (`sender_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_gift_receiver_id` FOREIGN KEY (`receiver_id`) REFERENCES `ez2on_character` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;