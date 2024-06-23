import React, { useState } from "react";
import * as Styles from './SearchWords.styles';

const SearchWord = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [serverResponse, setServerResponse] = useState([]);

    const handleInputChange = (event) => {
        const inputValue = event.target.value;
        setSearchTerm(inputValue);
    };

    const apisearch = async () => {
        try {
            const response = await fetch(`http://localhost:5015/Words/${searchTerm}`);
            const data = await response.json();
            console.log(data);
            setServerResponse(data);
        } catch (error) {
            console.error('Error checking word existence:', error);
            setServerResponse(['Error checking word existence']);
        }
    };

    const wordToDisplay = 'koogle';

    return (
        <Styles.Div>
            <Styles.Warper>
                <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Josefin+Sans:wght@300;400;700&display=swap" />
                <Styles.koogleText>
                    {wordToDisplay.split('').map((letter, index) => (
                    <Styles.ColorfulLetter key={index} index={index}>
                    {letter}
                    </Styles.ColorfulLetter>
                    ))}
                </Styles.koogleText>
                <Styles.InputAndButtonWarper>
                    <Styles.Input
                        type="text"
                        placeholder="Enter your word"
                        value={searchTerm}
                        onChange={handleInputChange}
                    />
                    <Styles.SearchButton onClick={apisearch}>
                        <Styles.SearchIcon src='/mingcute_search-fill.svg'/>
                    </Styles.SearchButton>
                </Styles.InputAndButtonWarper>
                <ul>
                    {serverResponse.map((tuple, index) => (
                        <Styles.StyledListItem key={index}>
                            <a href={tuple.Item1} target="_blank" rel="noopener noreferrer">
                                {`URL: ${tuple.Item1}, Count: ${tuple.Item2}`}
                            </a>
                        </Styles.StyledListItem>
                    ))}
                </ul>
            </Styles.Warper>
            <Styles.SettingsButton>
                <Styles.SetingsIcon src='/Vector.svg' alt="SearchIcon"/>
            </Styles.SettingsButton> 
        </Styles.Div>
    );
}

export default SearchWord;
