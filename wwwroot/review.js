document.addEventListener("DOMContentLoaded", function() {
    const ratingInputNumber = document.getElementById('rating-input');
    const ratingShowNumber  = document.getElementById('rating-show-number');

    document.getElementById('rating-stars-input').addEventListener('mouseenter', function(e){
        // TODO: Fix to use delegation, cannot trigger mouseenter for some reason in stars

        // console.log("mouseenter", e.target.classList);
        // if (e.target.classList.contains('bi-star')) {
            // console.log("target", e.target);
        // }
    });
    const inputStarsContanier = document.getElementById("rating-stars-input");
    const inputStars = document.querySelectorAll("#rating-stars-input .star");

    inputStars.forEach((inputStar)=>{
        inputStar.addEventListener('click', function(e){
            clickedStarNum = e.target.dataset.starNumber;
            ratingInputNumber.value = clickedStarNum;
            for(let i=0; i<inputStars.length; i++){
                inputStars[i].classList.remove("star-number-lower");

                if(inputStars[i].dataset.starNumber <= clickedStarNum){
                    inputStars[i].classList.remove("bi-star");
                    inputStars[i].classList.add("bi-star-fill");
                }
            }
        });

        inputStar.addEventListener('mouseenter', function(e){
            // const classes = e.target.classList;
            const hoverStarNum = e.target.dataset.starNumber; //e.target.dataset['star-number']
            ratingShowNumber.innerText = hoverStarNum;
            
            for(let i=0; i<inputStars.length; i++){
                inputStars[i].classList.remove("star-number-lower");
                inputStars[i].classList.remove("bi-star-fill");
                inputStars[i].classList.add("bi-star");
    
                if(inputStars[i].dataset.starNumber < hoverStarNum){
                    inputStars[i].classList.add("star-number-lower");
                }
                if(inputStars[i].dataset.starNumber <= hoverStarNum){
                    inputStars[i].classList.remove("bi-star");
                    inputStars[i].classList.add("bi-star-fill");
                }
            }

        });
    });
    inputStarsContanier.addEventListener('mouseleave', function(e){
        for(let i=0; i<inputStars.length; i++){
            inputStars[i].classList.remove("star-number-lower");

            if(inputStars[i].dataset.starNumber > ratingInputNumber.value){
                inputStars[i].classList.remove("bi-star-fill");
                inputStars[i].classList.add("bi-star");
            // }else if(inputStars[i].dataset.starNumber < ratingInputNumber.value){
            }else{
                inputStars[i].classList.add("bi-star-fill");
                inputStars[i].classList.remove("bi-star");
            }
        }
        ratingShowNumber.innerText = ratingInputNumber.value==0? "#" : ratingInputNumber.value;
    });


});
