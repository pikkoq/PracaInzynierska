import React, { useState } from 'react';
import defaultShoe from '../../assets/shoe.png';

const ShoeImage = ({ src, alt, className, style, onClick }) => {
    const [hasError, setHasError] = useState(false);

    const handleError = () => {
        setHasError(true);
    };

    return (
        <img 
            src={(!src || hasError) ? defaultShoe : src}
            alt={alt || "Shoe"} 
            className={className}
            style={style}
            onError={handleError} 
            onClick={onClick}
        />
    );
};

export default ShoeImage;
