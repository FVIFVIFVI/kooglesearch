import React, { useState } from 'react';

function UpdateUrls() {
    const [inputValue, setInputValue] = useState({
        url: '',
        time: '4444',
    });

    const handleInputChange = (e) => {
        setInputValue({
            ...inputValue,
            [e.target.name]: e.target.value,
        });
        console.log(JSON.stringify(inputValue));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Set the current timestamp to the 'time' property
        setInputValue({
            ...inputValue,
            time: new Date().toISOString(),
        });

        console.log(JSON.stringify(inputValue));
        try {
            const response = await fetch('http://localhost:5015/Urls', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    // Add other headers as needed
                },
                body: JSON.stringify(inputValue),
            });

            if (response.ok) {
                console.log('Data submitted successfully');
                // Handle successful response
            } else {
                console.error('Submission failed');
                // Handle errors
            }
        } catch (error) {
            console.error('There was an error submitting the data', error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>
                URL:
                <input type="text" name="url" value={inputValue.url} onChange={handleInputChange} />
            </label>
            <br />
            <button type="submit">Submit</button>
        </form>
    );
}

export default UpdateUrls;