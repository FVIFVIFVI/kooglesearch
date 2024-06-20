

import React from 'react';

async function Delete(urlId,colctionName) {
    try {
        // Validate that urlId is a non-empty string
        // if (typeof urlId !== 'string' || urlId.trim() === '') {
        //     throw new Error('Invalid URL ID');
        // }
        
    const response = await fetch(`http://localhost:5015/${colctionName}/${urlId}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      // Handle error responses here
      const errorMessage = await response.text();
      console.error(`Error deleting URL: ${errorMessage}`);
      // Handle the error based on your application's requirements
    } else {
      // Successful deletion
      console.log('URL deleted successfully.');
      // Optionally, update your UI or perform any additional actions
    }
  } catch (error) {
    // Handle other errors
    console.error(`Error deleting URL: ${error.message}`);
    // Handle the error based on your application's requirements
  }
}

function DeleteUrls() {
  const id_ = '65b6114c266ea15db8345f34';


  const handleDelete = async () => {
    // Call the deleteUrl function with the specific URL ID
    await Delete(id_,'Words');
  };

  return (
    <div>
      <button onClick={handleDelete}>Delete URL</button>
    </div>
  );
}

export default Delete;
