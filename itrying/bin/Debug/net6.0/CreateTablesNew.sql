-- MySQL Script generated by MySQL Workbench
-- Sun Oct  2 23:38:28 2022
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema catbd
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema catbd
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `catbd` DEFAULT CHARACTER SET utf8 ;
USE `catbd` ;

-- -----------------------------------------------------
-- Table `catbd`.`breed`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `catbd`.`breed` (
  `breed_id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`breed_id`),
  UNIQUE INDEX `breed_id_UNIQUE` (`breed_id` ASC) VISIBLE,
  UNIQUE INDEX `title_UNIQUE` (`title` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `catbd`.`pride`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `catbd`.`pride` (
  `pride_id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`pride_id`),
  UNIQUE INDEX `pride_id_UNIQUE` (`pride_id` ASC) VISIBLE,
  UNIQUE INDEX `title_UNIQUE` (`title` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `catbd`.`cat`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `catbd`.`cat` (
  `cat_id` INT NOT NULL AUTO_INCREMENT,
  `nickname` VARCHAR(45) NOT NULL,
  `breed_id` INT NOT NULL,
  `pride_id` INT NOT NULL,
  `age` INT NOT NULL,
  PRIMARY KEY (`cat_id`),
  UNIQUE INDEX `cat_id_UNIQUE` (`cat_id` ASC) VISIBLE,
  INDEX `fk_cat_breed1_idx` (`breed_id` ASC) VISIBLE,
  INDEX `fk_cat_pride1_idx` (`pride_id` ASC) VISIBLE,
  CONSTRAINT `fk_cat_breed1`
    FOREIGN KEY (`breed_id`)
    REFERENCES `catbd`.`breed` (`breed_id`)
    ON DELETE restrict
    ON UPDATE cascade,
  CONSTRAINT `fk_cat_pride1`
    FOREIGN KEY (`pride_id`)
    REFERENCES `catbd`.`pride` (`pride_id`)
    ON DELETE restrict
    ON UPDATE cascade)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `catbd`.`owner`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `catbd`.`owner` (
  `owner_id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`owner_id`),
  UNIQUE INDEX `owner_id_UNIQUE` (`owner_id` ASC) VISIBLE,
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `catbd`.`cat_owner`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `catbd`.`cat_owner` (
  `owner_id` INT NOT NULL,
  `cat_id` INT NOT NULL,
  INDEX `fk_cat_owner_cat_idx` (`cat_id` ASC) VISIBLE,
  PRIMARY KEY (`owner_id`, `cat_id`),
  CONSTRAINT `fk_cat_owner_cat`
    FOREIGN KEY (`cat_id`)
    REFERENCES `catbd`.`cat` (`cat_id`)
    ON DELETE cascade
    ON UPDATE cascade,
  CONSTRAINT `fk_cat_owner_owner1`
    FOREIGN KEY (`owner_id`)
    REFERENCES `catbd`.`owner` (`owner_id`)
    ON DELETE cascade
    ON UPDATE cascade)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `catbd`.`family`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `catbd`.`family` (
  `family_id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`family_id`),
  UNIQUE INDEX `family_id_UNIQUE` (`family_id` ASC) VISIBLE,
  UNIQUE INDEX `title_UNIQUE` (`title` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `catbd`.`owner_family`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `catbd`.`owner_family` (
  `owner_id` INT NOT NULL,
  `family_id` INT NOT NULL,
  INDEX `fk_owner_family_family1_idx` (`family_id` ASC) VISIBLE,
  PRIMARY KEY (`owner_id`, `family_id`),
  CONSTRAINT `fk_owner_family_owner1`
    FOREIGN KEY (`owner_id`)
    REFERENCES `catbd`.`owner` (`owner_id`)
    ON DELETE cascade
    ON UPDATE cascade,
  CONSTRAINT `fk_owner_family_family1`
    FOREIGN KEY (`family_id`)
    REFERENCES `catbd`.`family` (`family_id`)
    ON DELETE cascade
    ON UPDATE cascade)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
