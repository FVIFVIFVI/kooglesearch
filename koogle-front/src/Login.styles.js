import styled from 'styled-components';

export const Div = styled.div`
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
`;

export const Warper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 80%; 
  width: 80%;
  margin-bottom: 15%;
`;

export const TextWarper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
`;

export const LoginText = styled.text`
  color: #FFF;
  font-family: "Josefin Sans";
  font-size: 40px;
  font-style: normal;
  font-weight: 600;
  line-height: normal;
  position: fixed;
  bottom: 85%;
  right: 52%;
`;

export const koogleText = styled.text`
  font-family: "Josefin Sans";
  font-size: 1000%;
  font-style: normal;
  font-weight: 500;
  line-height: normal;
  display: flex;
  /* padding: 20px; */
  margin-bottom: 20%;
  margin: 50px;
`;

const getColor = (index) => {
  switch (index % 4) {
    case 0:
      return '#4285F4';
    case 1:
      return '#EA4537';
    case 2:
      return '#FBC21B';
    case 3:
      return '#34A853';
    default:
      return '#000000';
  }
};

export const ColorfulLetter = styled.span`
  color: ${(props) => getColor(props.index)};
  margin: 0 2px;
`;

export const InputAndButtonWarper = styled.div`
  display: flex;
  flex-direction: column;
  width: 50%;
`;

export const InputUserName = styled.input`
  padding: 20px;
  width: 100%;
  height: 15%;
  border-radius: 50px;
  margin: 10px;
`;

export const InputPassword = styled.input`
  padding: 20px;
  width: 100%;
  height: 15%;
  border-radius: 50px;
  margin: 10px;
`;

export const ConnectButton = styled.button`
  width: 8%;
  height: 10%;
  background-color: #FBC21B;
  margin-bottom: 3%;
  border-radius: 50px;
  color: #FFF;
  text-align: center;
  font-family: "Josefin Sans";
  font-size: 23px;
  font-style: normal;
  font-weight: 600;
`;

