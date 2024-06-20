import React from "react";
import * as Styles from './Login.styles';

const Login = () => {
    

    const wordToDisplay = 'koogle';

    return (
            <Styles.Div>
                <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Josefin+Sans:wght@300;400;700&display=swap" />
                <Styles.Warper>
                    <Styles.TextWarper>
                        <Styles.LoginText> login to </Styles.LoginText>
                        <Styles.koogleText>
                            {wordToDisplay.split('').map((letter, index) => (
                            <Styles.ColorfulLetter key={index} index={index}>
                            {letter}
                            </Styles.ColorfulLetter>
                            ))}
                        </Styles.koogleText>
                    </Styles.TextWarper>
                    <Styles.InputAndButtonWarper>
                        <Styles.InputUserName
                            type="text"
                            placeholder="user name"
                            // value={searchTerm}
                            // onChange={handleInputChange}
                        />
                        <Styles.InputPassword
                            type="text"
                            placeholder="password"
                        />
                    </Styles.InputAndButtonWarper>
                </Styles.Warper>
                <Styles.ConnectButton> connect </Styles.ConnectButton> 
            </Styles.Div>
    );
}

export default Login;