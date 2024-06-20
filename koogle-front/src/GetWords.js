
// ServerCommunicator.js
import React, { useState, useEffect } from 'react';

const GetWords = () => {
  const [serverResponse, setServerResponse] = useState('');

  useEffect(() => {
    // Function to make the server request
    const fetchData = async () => {
      try {
        const response = await fetch('http://localhost:5015/Words');
        const data = await response.text();
        console.log(data);
        setServerResponse(data);
      } catch (error) {
        console.error('Error fetching data from the server:', error);
      }
    };

    // Call the function when the component mounts
    fetchData();
  }, []); // Empty dependency array means it only runs once when the component mounts

  return (
    <div>
      <p>{serverResponse}</p>
    </div>
  );
};

export default GetWords;