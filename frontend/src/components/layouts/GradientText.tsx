import React from 'react';
import styled, { keyframes } from 'styled-components';

/**
 * Componente estilizado de contenedor que centra su contenido en la pantalla.
 * 
 * Este componente utiliza `styled-components` para crear un contenedor que
 * posiciona su contenido en el centro de la pantalla, tanto vertical como
 * horizontalmente. El texto dentro del contenedor está alineado al centro.
 */
const Container = styled.div`
  position: absolute;
  top: 50%;
  left: 50%;
  width: 100%;
  transform: translate(-50%, -50%);
  text-align: center;
`;

/**
 * Componente estilizado de texto que aplica un efecto de gradiente animado.
 * 
 * Este componente utiliza `styled-components` para crear un texto con un
 * gradiente de color animado. El gradiente se mueve de izquierda a derecha
 * y viceversa, creando un efecto visual atractivo. El texto es transparente
 * y el gradiente se aplica como fondo.
 */
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

/**
 * Componente de texto con gradiente animado.
 * 
 * Este componente muestra un texto con un efecto de gradiente animado y un enlace
 * de crédito en la esquina inferior derecha. Utiliza `styled-components` para
 * aplicar estilos y animaciones al texto.
 * 
 * @param {Object} props - Propiedades del componente.
 * @param {string} props.text - El texto que se mostrará con el efecto de gradiente.
 * @returns {JSX.Element} El componente de texto con gradiente.
 */
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