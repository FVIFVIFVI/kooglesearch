import React, { useState } from "react";
import * as Styles from './SearchWords.styles';

const SearchWord = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [serverResponse, setServerResponse] = useState([]);
    const [isPopupVisible, setIsPopupVisible] = useState(false);

    const handleInputChange = (event) => {
        const inputValue = event.target.value;
        setSearchTerm(inputValue);
    };

    const apisearch = async () => {
        try {
            const response = await fetch(`http://localhost:7122/Words/${searchTerm}`);
            const data = await response.json();
            console.log(data);
            setServerResponse(data);
        } catch (error) {
            console.error('Error checking word existence:', error);
            setServerResponse(['Error checking word existence']);
        }
    };

    const handleAddUrl = () => {
        setIsPopupVisible(!isPopupVisible);
    };

    const functionOne = async () => {
        try {
            const response = await fetch(`http://localhost:7122/api/HomeController1`, {
                method: 'POST'
            });
            if (response.ok) {
                console.log("Function One executed successfully");
            } else {
                console.error('Error executing Function One');
            }
        } catch (error) {
            console.error('Error executing Function One:', error);
        }
    };

    const functionTwo = async () => {
        try {
            const response = await fetch(`http://localhost:7122/api/HomeController1/calculate-pagerank`, {
                method: 'POST'
            });
            if (response.ok) {
                console.log("Function Two (Calculate PageRank) executed successfully");
            } else {
                console.error('Error executing Function Two');
            }
        } catch (error) {
            console.error('Error executing Function Two:', error);
        }
    };

    const functionThree = async () => {
        try {
            const response = await fetch(`https://localhost:7122/api/HomeController1/crawler`, {
                method: 'POST'
            });
            if (response.ok) {
                console.log("Function Three (Crawler) executed successfully");
            } else {
                console.error('Error executing Function Three');
            }
        } catch (error) {
            console.error('Error executing Function Three:', error);
        }
    };

    const functionFour = () => {
        console.log("Function Four executed");
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
            <Styles.SettingsButton onClick={handleAddUrl}>
                <Styles.SetingsIcon src='/Vector.svg' alt="SettingsIcon"/>
            </Styles.SettingsButton>
            {isPopupVisible && (
                <Styles.Popup>
                    <Styles.PopupButton onClick={functionOne}>add new url</Styles.PopupButton>
                    <Styles.PopupButton onClick={functionTwo}>cul pagerank</Styles.PopupButton>
                    <Styles.PopupButton onClick={functionThree}>run crawler</Styles.PopupButton>
                    {/* <Styles.PopupButton onClick={functionFour}>Function Four</Styles.PopupButton> */}
                </Styles.Popup>
            )}
        </Styles.Div>
    );
}

export default SearchWord;
