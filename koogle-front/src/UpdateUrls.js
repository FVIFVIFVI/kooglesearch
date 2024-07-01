import React from 'react';

function UpdateUrls() {
    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('http://localhost:5015/api/home/trigger-get-async', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    // Add other headers as needed
                },
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
            <button type="submit">123</button>
        </form>
    );
}

export default UpdateUrls;
