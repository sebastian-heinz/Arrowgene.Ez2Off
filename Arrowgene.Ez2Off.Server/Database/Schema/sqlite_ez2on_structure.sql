CREATE TABLE IF NOT EXISTS `setting` (
  `key`   TEXT NOT NULL,
  `value` TEXT NOT NULL,
  PRIMARY KEY (`key`)
);

CREATE TABLE IF NOT EXISTS `account` (
  `id`               INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `name`             TEXT                              NOT NULL,
  `normal_name`      TEXT                              NOT NULL,
  `hash`             TEXT                              NOT NULL,
  `mail`             TEXT                              NOT NULL,
  `mail_verified`    INTEGER                           NOT NULL,
  `mail_verified_at` DATETIME DEFAULT NULL,
  `mail_token`       TEXT     DEFAULT NULL,
  `password_token`   TEXT     DEFAULT NULL,
  `state`            INTEGER                           NOT NULL,
  `last_login`       DATETIME DEFAULT NULL,
  `created`          DATETIME                          NOT NULL,
  CONSTRAINT `uq_account_name` UNIQUE (`name`),
  CONSTRAINT `uq_account_normal_name` UNIQUE (`normal_name`),
  CONSTRAINT `uq_account_mail` UNIQUE (`mail`)
);

CREATE TABLE IF NOT EXISTS `incident` (
  `id`          INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `account_id`  INTEGER                           NOT NULL,
  `type`        INTEGER                           NOT NULL,
  `description` TEXT                              NOT NULL,
  `created`     DATETIME                          NOT NULL,
  CONSTRAINT `fk_incident_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
);
  
CREATE TABLE IF NOT EXISTS `identification` (
  `id`          INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `account_id`  INTEGER                           NOT NULL,
  `ip`          TEXT                              NOT NULL,
  `hardware_id` TEXT                                        DEFAULT NULL,
  CONSTRAINT `uq_ip_account_id_ip_hardware_id` UNIQUE (`account_id`, `ip`, `hardware_id`),
  CONSTRAINT `fk_identification_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_character` (
  `id`             INTEGER NOT NULL,
  `name`           TEXT    NOT NULL,
  `sex`            INTEGER NOT NULL,
  `level`          INTEGER NOT NULL,
  `ruby_exr`       INTEGER NOT NULL,
  `street_exr`     INTEGER NOT NULL,
  `club_exr`       INTEGER NOT NULL,
  `exp`            INTEGER NOT NULL,
  `coin`           INTEGER NOT NULL,
  `cash`           INTEGER NOT NULL,
  `max_combo`      INTEGER NOT NULL,
  `ruby_wins`      INTEGER NOT NULL,
  `street_wins`    INTEGER NOT NULL,
  `club_wins`      INTEGER NOT NULL,
  `ruby_loses`     INTEGER NOT NULL,
  `street_loses`   INTEGER NOT NULL,
  `club_loses`     INTEGER NOT NULL,
  `premium`        INTEGER NOT NULL,
  `dj_points`      INTEGER NOT NULL,
  `dj_points_plus` INTEGER NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_ez2on_character_id` FOREIGN KEY (`id`) REFERENCES `account` (`id`),
  CONSTRAINT `uq_ez2on_character_name` UNIQUE (`name`)
);

CREATE TABLE IF NOT EXISTS `ez2on_song` (
  `id`               INTEGER NOT NULL,
  `name`             TEXT    NOT NULL,
  `category`         INTEGER NOT NULL,
  `duration`         TEXT    NOT NULL,
  `bpm`              INTEGER NOT NULL,
  `file_name`        TEXT    NOT NULL,
  `ruby_ez_exr`      INTEGER NOT NULL,
  `ruby_ez_notes`    INTEGER NOT NULL,
  `ruby_nm_exr`      INTEGER NOT NULL,
  `ruby_nm_notes`    INTEGER NOT NULL,
  `ruby_hd_exr`      INTEGER NOT NULL,
  `ruby_hd_notes`    INTEGER NOT NULL,
  `ruby_shd_exr`     INTEGER NOT NULL,
  `ruby_shd_notes`   INTEGER NOT NULL,
  `street_ez_exr`    INTEGER NOT NULL,
  `street_ez_notes`  INTEGER NOT NULL,
  `street_nm_exr`    INTEGER NOT NULL,
  `street_nm_notes`  INTEGER NOT NULL,
  `street_hd_exr`    INTEGER NOT NULL,
  `street_hd_notes`  INTEGER NOT NULL,
  `street_shd_exr`   INTEGER NOT NULL,
  `street_shd_notes` INTEGER NOT NULL,
  `club_ez_exr`      INTEGER NOT NULL,
  `club_ez_notes`    INTEGER NOT NULL,
  `club_nm_exr`      INTEGER NOT NULL,
  `club_nm_notes`    INTEGER NOT NULL,
  `club_hd_exr`      INTEGER NOT NULL,
  `club_hd_notes`    INTEGER NOT NULL,
  `club_shd_exr`     INTEGER NOT NULL,
  `club_shd_notes`   INTEGER NOT NULL,
  `measure_scale`    REAL    NOT NULL,
  `judgment_kool`    INTEGER NOT NULL,
  `judgment_cool`    INTEGER NOT NULL,
  `judgment_good`    INTEGER NOT NULL,
  `judgment_miss`    INTEGER NOT NULL,
  `gauge_cool`       REAL    NOT NULL,
  `gauge_good`       REAL    NOT NULL,
  `gauge_miss`       REAL    NOT NULL,
  `gauge_fail`       REAL    NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_radiomix` (
  `id`                  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `b`                   INTEGER                           NOT NULL,
  `c`                   INTEGER                           NOT NULL,
  `d`                   INTEGER                           NOT NULL,
  `e`                   INTEGER                           NOT NULL,
  `song_1_id`           INTEGER                           NOT NULL,
  `song_1_ruby_notes`   INTEGER                           NOT NULL,
  `song_1_street_notes` INTEGER                           NOT NULL,
  `song_1_club_notes`   INTEGER                           NOT NULL,
  `song_1_club8k_notes` INTEGER                           NOT NULL,
  `song_2_id`           INTEGER                           NOT NULL,
  `song_2_ruby_notes`   INTEGER                           NOT NULL,
  `song_2_street_notes` INTEGER                           NOT NULL,
  `song_2_club_notes`   INTEGER                           NOT NULL,
  `song_2_club8k_notes` INTEGER                           NOT NULL,
  `song_3_id`           INTEGER                           NOT NULL,
  `song_3_ruby_notes`   INTEGER                           NOT NULL,
  `song_3_street_notes` INTEGER                           NOT NULL,
  `song_3_club_notes`   INTEGER                           NOT NULL,
  `song_3_club8k_notes` INTEGER                           NOT NULL,
  `song_4_id`           INTEGER                           NOT NULL,
  `song_4_ruby_notes`   INTEGER                           NOT NULL,
  `song_4_street_notes` INTEGER                           NOT NULL,
  `song_4_club_notes`   INTEGER                           NOT NULL,
  `song_4_club8k_notes` INTEGER                           NOT NULL,
  CONSTRAINT `fk_ez2on_radiomix_id` FOREIGN KEY (`id`) REFERENCES `ez2on_song` (`id`),
  CONSTRAINT `fk_ez2on_radiomix_song_1_id` FOREIGN KEY (`song_1_id`) REFERENCES `ez2on_song` (`id`),
  CONSTRAINT `fk_ez2on_radiomix_song_2_id` FOREIGN KEY (`song_2_id`) REFERENCES `ez2on_song` (`id`),
  CONSTRAINT `fk_ez2on_radiomix_song_3_id` FOREIGN KEY (`song_3_id`) REFERENCES `ez2on_song` (`id`),
  CONSTRAINT `fk_ez2on_radiomix_song_4_id` FOREIGN KEY (`song_4_id`) REFERENCES `ez2on_song` (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_game` (
  `id`         INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `song_id`    INTEGER                           NOT NULL,
  `group_type` INTEGER                           NOT NULL,
  `type`       INTEGER                           NOT NULL,
  `name`       TEXT                              NOT NULL,
  `created`    DATETIME                          NOT NULL,
  `mode`       INTEGER                           NOT NULL,
  `difficulty` INTEGER                           NOT NULL,
  CONSTRAINT `fk_ez2on_game_song_id` FOREIGN KEY (`song_id`) REFERENCES `ez2on_song` (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_item` (
  `id`              INTEGER NOT NULL,
  `name`            TEXT    NOT NULL,
  `effect`          TEXT    NOT NULL,
  `image`           TEXT    NOT NULL,
  `duration`        INTEGER NOT NULL,
  `price`           INTEGER NOT NULL,
  `level`           INTEGER NOT NULL,
  `exp_plus`        INTEGER NOT NULL,
  `coin_plus`       INTEGER NOT NULL,
  `hp_plus`         INTEGER NOT NULL,
  `resilience_plus` INTEGER NOT NULL,
  `defense_plus`    INTEGER NOT NULL,
  `type`            INTEGER NOT NULL,
  `enabled`         INTEGER NOT NULL,
  `currency`        INTEGER NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_inventory` (
  `id`            INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `character_id`  INTEGER                           NOT NULL,
  `item_id`       INTEGER                           NOT NULL,
  `purchase_date` DATETIME                          NOT NULL,
  `slot`          INTEGER                           NOT NULL,
  `equipped`      INTEGER                           NOT NULL,
  `equip_date`    DATETIME                                    DEFAULT NULL,
  `expire_date`   DATETIME                                    DEFAULT NULL,
  CONSTRAINT `fk_ez2on_inventory_character_id` FOREIGN KEY (`character_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_inventory_item_id` FOREIGN KEY (`item_id`) REFERENCES `ez2on_item` (`id`)
);
CREATE INDEX IF NOT EXISTS `idx_ez2on_inventory_expire_date` ON `ez2on_inventory` (`expire_date`);

CREATE TABLE IF NOT EXISTS `ez2on_quest` (
  `id`      INTEGER NOT NULL,
  `title`   TEXT    NOT NULL,
  `mission` TEXT    NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_score` (
  `id`           INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `game_id`      INTEGER                           NOT NULL,
  `character_id` INTEGER                           NOT NULL,
  `song_id`      INTEGER                           NOT NULL,
  `difficulty`   INTEGER                           NOT NULL,
  `stage_clear`  INTEGER                           NOT NULL,
  `max_combo`    INTEGER                           NOT NULL,
  `kool`         INTEGER                           NOT NULL,
  `cool`         INTEGER                           NOT NULL,
  `good`         INTEGER                           NOT NULL,
  `miss`         INTEGER                           NOT NULL,
  `fail`         INTEGER                           NOT NULL,
  `raw_score`    INTEGER                           NOT NULL,
  `rank`         INTEGER                           NOT NULL,
  `total_notes`  INTEGER                           NOT NULL,
  `combo_type`   INTEGER                           NOT NULL,
  `total_score`  INTEGER                           NOT NULL,
  `note_effect`  INTEGER                           NOT NULL,
  `fade_effect`  INTEGER                           NOT NULL,
  `created`      DATETIME                          NOT NULL,
  `mode`         INTEGER                           NOT NULL,
  `incident`     INTEGER                           NOT NULL,
  CONSTRAINT `fk_ez2on_score_game_id` FOREIGN KEY (`game_id`) REFERENCES `ez2on_game` (`id`),
  CONSTRAINT `fk_ez2on_score_character_id` FOREIGN KEY (`character_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_score_song_id` FOREIGN KEY (`song_id`) REFERENCES `ez2on_song` (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_score_incident` (
  `score_id`      INTEGER NOT NULL,
  `incident_id`   INTEGER NOT NULL,  
  CONSTRAINT `uq_ez2on_score_incident_score_id_incident_id` UNIQUE (`score_id`, `incident_id`),
  CONSTRAINT `fk_ez2on_score_incident_score_id` FOREIGN KEY (`score_id`) REFERENCES `ez2on_score` (`id`),
  CONSTRAINT `fk_ez2on_score_incident_incident_id` FOREIGN KEY (`incident_id`) REFERENCES `incident` (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_setting` (
  `character_id`      INTEGER NOT NULL,
  `ruby_key_on_1`     INTEGER NOT NULL,
  `ruby_key_on_2`     INTEGER NOT NULL,
  `ruby_key_on_3`     INTEGER NOT NULL,
  `ruby_key_on_4`     INTEGER NOT NULL,
  `ruby_key_ac_1`     INTEGER NOT NULL,
  `ruby_key_ac_2`     INTEGER NOT NULL,
  `ruby_key_ac_3`     INTEGER NOT NULL,
  `ruby_key_ac_4`     INTEGER NOT NULL,
  `street_key_on_1`   INTEGER NOT NULL,
  `street_key_on_2`   INTEGER NOT NULL,
  `street_key_on_3`   INTEGER NOT NULL,
  `street_key_on_4`   INTEGER NOT NULL,
  `street_key_on_5`   INTEGER NOT NULL,
  `street_key_ac_1`   INTEGER NOT NULL,
  `street_key_ac_2`   INTEGER NOT NULL,
  `street_key_ac_3`   INTEGER NOT NULL,
  `street_key_ac_4`   INTEGER NOT NULL,
  `street_key_ac_5`   INTEGER NOT NULL,
  `club_key_on_1`     INTEGER NOT NULL,
  `club_key_on_2`     INTEGER NOT NULL,
  `club_key_on_3`     INTEGER NOT NULL,
  `club_key_on_4`     INTEGER NOT NULL,
  `club_key_on_5`     INTEGER NOT NULL,
  `club_key_on_6`     INTEGER NOT NULL,
  `club_key_ac_1`     INTEGER NOT NULL,
  `club_key_ac_2`     INTEGER NOT NULL,
  `club_key_ac_3`     INTEGER NOT NULL,
  `club_key_ac_4`     INTEGER NOT NULL,
  `club_key_ac_5`     INTEGER NOT NULL,
  `club_key_ac_6`     INTEGER NOT NULL,
  `volume_menu_music` INTEGER NOT NULL,
  `volume_menu_sfx`   INTEGER NOT NULL,
  `volume_game_music` INTEGER NOT NULL,
  `volume_game_sfx`   INTEGER NOT NULL,
  `bga_settings`      INTEGER NOT NULL,
  `skin_position`     INTEGER NOT NULL,
  `skin_type`         INTEGER NOT NULL,
  PRIMARY KEY (`character_id`),
  CONSTRAINT `fk_ez2on_setting_character_id` FOREIGN KEY (`character_id`) REFERENCES `ez2on_character` (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_message` (
  `id`          INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `sender_id`   INTEGER                           NOT NULL,
  `receiver_id` INTEGER                           NOT NULL,
  `content`     TEXT                              NOT NULL,
  `send_at`     DATETIME                          NOT NULL,
  `read`        INTEGER                           NOT NULL,
  CONSTRAINT `fk_ez2on_message_sender_id` FOREIGN KEY (`sender_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_message_receiver_id` FOREIGN KEY (`receiver_id`) REFERENCES `ez2on_character` (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_friend` (
  `id`                  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `character_id`        INTEGER                           NOT NULL,
  `friend_character_id` INTEGER                           NOT NULL,
  CONSTRAINT `uq_ez2on_friend_character_id_friend_character_id` UNIQUE (`character_id`, `friend_character_id`),
  CONSTRAINT `fk_ez2on_friend_character_id` FOREIGN KEY (`character_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_friend_friend_character_id` FOREIGN KEY (`friend_character_id`) REFERENCES `ez2on_character` (`id`)
);

CREATE TABLE IF NOT EXISTS `ez2on_rank` (
  `id`       INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `game_id`  INTEGER                           NOT NULL,
  `score_id` INTEGER                           NOT NULL,
  `ranking`  INTEGER                           NOT NULL,
  `team`     INTEGER                           NOT NULL,
  CONSTRAINT `fk_ez2on_rank_game_id` FOREIGN KEY (`game_id`) REFERENCES `ez2on_game` (`id`),
  CONSTRAINT `fk_ez2on_rank_score_id` FOREIGN KEY (`score_id`) REFERENCES `ez2on_score` (`id`)
);  

CREATE TABLE IF NOT EXISTS `ez2on_gift` (
  `id`          INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `item_id`     INTEGER                           NOT NULL,
  `sender_id`   INTEGER                           NOT NULL,
  `receiver_id` INTEGER                           NOT NULL,
  `send_at`     DATETIME                          NOT NULL,
  `read`        INTEGER                           NOT NULL,
  `expire_date` DATETIME                                    DEFAULT NULL,
  CONSTRAINT `fk_ez2on_gift_item_id` FOREIGN KEY (`item_id`) REFERENCES `ez2on_item` (`id`),
  CONSTRAINT `fk_ez2on_gift_sender_id` FOREIGN KEY (`sender_id`) REFERENCES `ez2on_character` (`id`),
  CONSTRAINT `fk_ez2on_gift_receiver_id` FOREIGN KEY (`receiver_id`) REFERENCES `ez2on_character` (`id`)
);
CREATE INDEX IF NOT EXISTS `idx_ez2on_gift_expire_date` ON `ez2on_gift` (`expire_date`);