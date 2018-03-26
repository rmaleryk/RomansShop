import {Directive, ElementRef, Renderer2, Input} from '@angular/core';
 
@Directive({
    selector: '[guid]'
})
export class GuidDirective {

    constructor(private elementRef: ElementRef, 
                private renderer: Renderer2) {
    }

    @Input() set guid(guid: string) {
        this.renderer.setStyle(this.elementRef.nativeElement, "height", "1.5em");
        this.renderer.setStyle(this.elementRef.nativeElement, "max-width", "60px");
        this.renderer.setStyle(this.elementRef.nativeElement, "overflow", "hidden");
        this.renderer.setStyle(this.elementRef.nativeElement, "text-overflow", "ellipsis");
        
        this.elementRef.nativeElement.innerHTML = guid;
        this.elementRef.nativeElement.title = guid;
    }
}