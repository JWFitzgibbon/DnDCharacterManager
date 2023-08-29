import { Fragment } from "react";
import Summary from "./Summary/Summary";
import CharacterSheet from "./CharacterSheet/CharacterSheet";

function Content() {
    return (
        <Fragment>
            <Summary />
            <CharacterSheet />
        </Fragment>
    )
}

export default Content;