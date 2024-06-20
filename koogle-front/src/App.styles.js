import styled from 'styled-components';
import backgroundImage from './painting-mountain-lake-with-mountain-background.jpg';

export const BackgroundImage = styled.div`
  height: 100vh;
  width: 100vw;
  display: flex;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: url(${backgroundImage});
  background-size: cover;
  margin: 0;
  padding: 0;
  overflow: hidden;
`;
