class ElementBuilder{
    constructor(element){
        this.el = document.createElement(element);
    }
    withClassName(className) {
        this.el.className = className;
        return this;
    }
    withInnerHTML(innetHTML) {
        this.el.innerHTML = innetHTML;
        return this;
    }
    withEventListener(eventName, callback) {
        this.el.addEventListener(eventName, callback);
        return this;
    }
    withDraggable(draggable) {
        this.el.draggable = draggable;
        return this;
    }
    withAttribute(attribute, value) {
        this.el.setAttribute(attribute, value);
        return this;
    }
    withStyle(attribute, value) {
        this.el.style[attribute] = value;
        return this;
    }
    withId(id) {
        this.el.id = id; 
        return this;
    }
    build() {
        return this.el;
    }
}

class ImageBuilder extends ElementBuilder{
    constructor() {
        super();
        this.el = document.createElement('img');
    }
    withSrc(src) {
        this.el.src = src;
        return this;
    }
}

class InputBuilder extends ElementBuilder {
    constructor() {
        super();
        this.el = document.createElement('input');
    }
    withName(name) {
        this.el.name = name;
        return this;
    }
    withType(type) {
        this.el.type = type;
        return this;
    }
    withValue(value) {
        this.el.value = value;
        return this;
    }
    withRole(role) {
        this.el.role = role;
        return this;
    }
}

class CardBuilder extends ElementBuilder{
    constructor(type) {
        super();
        this.el = document.createElement('div');
        this.el.className = 'card ' + type + '-card'
        this.bodyContent = new ElementBuilder('div').withClassName('card-body-content').build();
        this.cardBody = new ElementBuilder('div')
            .withClassName('card-body')
            .build();
    }
    withTitle(title) {
        this.cardTitle = new ElementBuilder('div')
            .withClassName('card-title')
            .withInnerHTML(title)
            .build();
        return this;
    }
    withDescription(description) {
        this.cardDescription = new ElementBuilder('p')
            .withClassName('card-text')
            .withInnerHTML(description)
            .build();
        return this;
    }
    withImage(src) {
        this.img = new ImageBuilder()
            .withClassName('card-img-top')
            .withSrc(src)
            .build();
        return this;
    }
    addToBody(newElement) {
        this.bodyContent.append(newElement);
        return this;
    }
    addToCard(newElement) {
        this.el.append(newElement);
        return this;
    }
    getCardBody() {
        return this.cardBody;
    }
    build() {
        if(this.img != undefined)
            this.el.append(this.img);
        if(this.cardTitle!=undefined)
            this.cardBody.append(this.cardTitle);
        if(this.cardDescription!=undefined)
            this.cardBody.append(this.cardDescription);
        
        this.cardBody.append(this.bodyContent);
        this.el.append(this.cardBody);
        return this.el;
    }
}