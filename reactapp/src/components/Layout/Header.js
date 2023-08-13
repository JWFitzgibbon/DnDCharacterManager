import { Fragment } from "react";
import headerImage from "../../assets/dnd.avif";
import dndLogo from "../../assets/dnd-hub-logo.avif";
import classes from "./Header.module.css";

// Image source: https://dnd.wizards.com/resources/press-assets
function Header(props) {
  return (
    <Fragment>
      <header className={classes.header}>
        <img src={dndLogo} alt="The official D&D logo" />
        <h1>Character Manager</h1>
      </header>
      <div className={classes["main-image"]}>
        <img src={headerImage} alt="A character fighting skeletons" />
      </div>
    </Fragment>
  );
}

export default Header;
