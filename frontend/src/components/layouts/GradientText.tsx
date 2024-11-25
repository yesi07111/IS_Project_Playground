import React from 'react';
import styled, { keyframes } from 'styled-components';

const Container = styled.div`
  position: absolute;
  top: 50%;
  left: 50%;
  width: 100%;
  transform: translate(-50%, -50%);
  text-align: center;
`;

const Text = styled.div`
  display: block;
  font-family: 'Ubuntu', sans-serif;
  text-transform: uppercase;
  letter-spacing: 0.2em;
  font-size: 1.3em;
  line-height: 2;
  font-weight: 300;
  color: transparent;
  background: linear-gradient(90deg, #ff0000, #00ff00, #0000ff, #ff0000);
  background-size: 300% 300%;
  -webkit-background-clip: text;
  background-clip: text;
  animation: ${keyframes`
    0% { background-position: 0% 50%; }
    50% { background-position: 100% 50%; }
    100% { background-position: 0% 50%; }
  `} 5s infinite;
`;

const GradientText: React.FC<{ text: string }> = ({ text }) => {
    return (
        <Container>
            <Text className="txt">
                {text}
            </Text>
            <a target="_blank" href="https://www.hendrysadrak.com" style={{ position: 'absolute', bottom: '10px', right: '10px', color: '#eee', fontSize: '15px', lineHeight: '15px', textDecoration: 'none' }}>
                @hendrysadrak
            </a>
        </Container>
    );
};

export default GradientText;

// import React, { useEffect } from 'react';
// import styled, { keyframes } from 'styled-components';

// const Container = styled.div`
//   position: absolute;
//   top: 50%;
//   left: 50%;
//   width: 100%;
//   transform: translate(-50%, -50%);
//   text-align: center;
// `;

// const Text = styled.div`
//   display: block;
//   font-family: 'Ubuntu', sans-serif;
//   text-transform: uppercase;
//   letter-spacing: 0.2em;
//   font-size: 1.3em;
//   line-height: 2;
//   font-weight: 300;
//   color: #fefefe;
// `;

// const animTextFlowKeys = keyframes`
//   0% { color: hsla(0, 60%, 60%, 1); }
//   20% { color: hsla(72, 60%, 60%, 1); }
//   40% { color: hsla(144, 60%, 60%, 1); }
//   60% { color: hsla(216, 60%, 60%, 1); }
//   80% { color: hsla(288, 60%, 60%, 1); }
//   100% { color: hsla(360, 60%, 60%, 1); }
// `;

// const AnimatedSpan = styled.span`
//   animation: ${animTextFlowKeys} 5s infinite alternate forwards;
// `;

// const GradientText: React.FC<{ text: string }> = ({ text }) => {
//     useEffect(() => {
//         const textElement = document.querySelector('.txt');
//         if (textElement) {
//             const chars = text.split('');
//             textElement.innerHTML = chars.map(char => `<span>${char}</span>`).join('');
//         }
//     }, [text]);

//     return (
//         <Container>
//             <Text className="txt">
//                 {text.split('').map((char, index) => (
//                     <AnimatedSpan key={index}>{char}</AnimatedSpan>
//                 ))}
//             </Text>
//             <a target="_blank" href="https://www.hendrysadrak.com" style={{ position: 'absolute', bottom: '10px', right: '10px', color: '#eee', fontSize: '15px', lineHeight: '15px', textDecoration: 'none' }}>
//                 @hendrysadrak
//             </a>
//         </Container>
//     );
// };

// export default GradientText;