import React from 'react';
import * as Styles from './App.styles';
import SearchWord from './SearchWord.js';
import Login from './Login.js';
import UpdateUrls from './UpdateUrls.js';

function App() {
  return (
    <Styles.BackgroundImage>
      <SearchWord />
      {/* <Login /> */}
      <UpdateUrls />   </Styles.BackgroundImage>
  );
}

export default App;
