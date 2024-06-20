import React from 'react';
import * as Styles from './App.styles';
import SearchWord from './SearchWord.js';
import Login from './Login.js';

function App() {


  return (
    <Styles.BackgroundImage>
      <SearchWord />
      {/* <Login /> */}
    </Styles.BackgroundImage>
  );
}

export default App;
